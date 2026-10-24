import { useCallback, useEffect, useRef, useState } from 'react';
import { PROGRESS_EVENT, PROGRESS_KEY } from './config';
import { emptyProgress, getBrowserStorage, loadProgress, saveProgress } from './storage';

export default function useProgress() {
  const [progress, setProgress] = useState(null);
  const [available, setAvailable] = useState(true);
  const current = useRef(null);

  const set = useCallback((p) => {
    current.current = p;
    setProgress(p);
  }, []);

  useEffect(() => {
    const storage = getBrowserStorage();
    setAvailable(storage !== null);
    const reload = () => set(loadProgress(storage));
    const onStorage = (e) => {
      if (e.key === null || e.key === PROGRESS_KEY) reload();
    };
    reload();
    window.addEventListener(PROGRESS_EVENT, reload);
    window.addEventListener('storage', onStorage);
    return () => {
      window.removeEventListener(PROGRESS_EVENT, reload);
      window.removeEventListener('storage', onStorage);
    };
  }, [set]);

  const replace = useCallback(
    (p) => {
      set(p);
      saveProgress(p);
    },
    [set],
  );

  const update = useCallback(
    (fn) => {
      replace(fn(current.current ?? emptyProgress()));
    },
    [replace],
  );

  return { progress, update, replace, available };
}
