// Example code and CI output snapshots for `output` questions, relative to website/examples/.
const context = require.context('!!raw-loader!../../../examples', true, /\.(cs|out\.txt)$/);

function read(path) {
  try {
    const mod = context(`./${path}`);
    return typeof mod === 'string' ? mod : mod.default;
  } catch (error) {
    return undefined;
  }
}

export function getExample(path) {
  const code = read(path);
  if (code === undefined) return undefined;
  // Snippets (*.snippet.cs) are not run by CI and have no snapshot.
  if (path.endsWith('.snippet.cs')) return {code, expected: undefined};
  const expected = read(path.replace(/\.cs$/, '.out.txt'));
  if (expected === undefined) return undefined;
  return {code, expected};
}
