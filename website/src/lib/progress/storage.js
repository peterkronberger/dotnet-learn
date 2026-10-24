import { PROGRESS_EVENT, PROGRESS_KEY } from './config';
import { validateProgress } from './exportImport';

export function emptyProgress() {
  return { version: 1, questions: {}, lessons: {}, activity: {} };
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

// Never throws.
export function loadProgress(storage = getBrowserStorage()) {
  if (!storage) return emptyProgress();
  try {
    const raw = storage.getItem(PROGRESS_KEY);
    if (raw === null) return emptyProgress();
    try {
      const result = validateProgress(JSON.parse(raw));
      if (result.ok) return result.progress;
    } catch {
      // fall through
    }
    // Broken data is not overwritten until the next saveProgress.
    return emptyProgress();
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
