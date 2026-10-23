import { LEGACY_PREFIX, MAX_QUESTION_ID_LENGTH, PROGRESS_EVENT, PROGRESS_KEY } from './config';
import { isValidIsoDate, toLocalDate } from './dates';
import { validateProgress } from './exportImport';

export function emptyProgress() {
  return { version: 1, questions: {} };
}

// null on SSR or when access throws (blocked storage, private mode).
export function getBrowserStorage() {
  try {
    if (typeof window === 'undefined' || !window.localStorage) return null;
    return window.localStorage;
  } catch {
    return null;
  }
}

export function createMemoryStorage(initial = {}) {
  const map = new Map(Object.entries(initial));
  return {
    getItem: (k) => (map.has(k) ? map.get(k) : null),
    setItem: (k, v) => {
      map.set(k, String(v));
    },
    removeItem: (k) => {
      map.delete(k);
    },
    key: (i) => Array.from(map.keys())[i] ?? null,
    get length() {
      return map.size;
    },
    clear: () => map.clear(),
  };
}

function legacyDate(lastAttempt, today) {
  if (typeof lastAttempt !== 'string') return today;
  const d = new Date(lastAttempt);
  if (Number.isNaN(d.getTime())) return today;
  const local = toLocalDate(d);
  return isValidIsoDate(local) ? local : today;
}

// One-time migration of dotnet-learn:quiz:<id> keys. Returns null if there is nothing to migrate.
// Unreadable legacy entries are skipped and deleted together with the usable ones (they are of no use).
// Legacy keys are only removed after the v1 data was written successfully.
export function migrateLegacy(storage, legacyIdMap = {}, today = toLocalDate()) {
  if (!storage) return null;
  try {
    const legacyKeys = [];
    for (let i = 0; i < storage.length; i++) {
      const k = storage.key(i);
      if (k && k.startsWith(LEGACY_PREFIX)) legacyKeys.push(k);
    }
    if (legacyKeys.length === 0) return null;

    const progress = emptyProgress();
    for (const key of legacyKeys) {
      try {
        const old = JSON.parse(storage.getItem(key));
        const oldId = key.slice(LEGACY_PREFIX.length);
        const id = Object.prototype.hasOwnProperty.call(legacyIdMap, oldId) ? legacyIdMap[oldId] : oldId;
        if (!old || typeof old.correct !== 'boolean') continue;
        if (typeof id !== 'string' || id.length === 0 || id.length > MAX_QUESTION_ID_LENGTH) continue;
        const attempts = Number.isInteger(old.attempts) && old.attempts >= 1 ? old.attempts : 1;
        progress.questions[id] = {
          box: old.correct ? 2 : 1,
          lastReviewed: legacyDate(old.lastAttempt, today),
          attempts,
          correctCount: old.correct ? 1 : 0,
          lastResult: old.correct ? 'correct' : 'wrong',
        };
      } catch {
        // skip unreadable entry
      }
    }
    storage.setItem(PROGRESS_KEY, JSON.stringify(progress));
    for (const key of legacyKeys) storage.removeItem(key);
    return progress;
  } catch {
    return null;
  }
}

// Never throws.
export function loadProgress(storage = getBrowserStorage(), { legacyIdMap = {} } = {}) {
  if (!storage) return emptyProgress();
  try {
    const raw = storage.getItem(PROGRESS_KEY);
    if (raw !== null) {
      try {
        const result = validateProgress(JSON.parse(raw));
        if (result.ok) return result.progress;
      } catch {
        // fall through
      }
      // Broken v1 data is not overwritten until the next saveProgress.
      return emptyProgress();
    }
    return migrateLegacy(storage, legacyIdMap) ?? emptyProgress();
  } catch {
    return emptyProgress();
  }
}

// Never throws; dispatches PROGRESS_EVENT on success when a window exists.
export function saveProgress(progress, storage = getBrowserStorage()) {
  if (!storage) return false;
  try {
    storage.setItem(PROGRESS_KEY, JSON.stringify(progress));
  } catch {
    return false;
  }
  try {
    if (typeof window !== 'undefined') window.dispatchEvent(new Event(PROGRESS_EVENT));
  } catch {
    // ignore
  }
  return true;
}
