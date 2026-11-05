// Recovers from "Loading chunk N failed" (for example Mermaid after a new deploy:
// the open page still points to old hashed files). Reloads once per session.
const KEY = 'dotnet-learn:chunk-reload';

function isChunkError(reason) {
  const text = String((reason && (reason.message || reason.name)) || reason || '');
  return /Loading chunk .* failed|ChunkLoadError|Loading CSS chunk/i.test(text);
}

function reloadOnce() {
  try {
    if (sessionStorage.getItem(KEY)) return;
    sessionStorage.setItem(KEY, '1');
  } catch {
    return;
  }
  window.location.reload();
}

if (typeof window !== 'undefined') {
  window.addEventListener('unhandledrejection', (e) => {
    if (isChunkError(e.reason)) reloadOnce();
  });
  window.addEventListener('error', (e) => {
    if (isChunkError(e.error || e.message)) reloadOnce();
  });
}
