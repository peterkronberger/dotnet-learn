// Fold markers in .cs files. Must behave like CodeFolds.Parse in playground/CodeFolds.cs.
// The exact lines (trimmed) "// @fold-start" and "// @fold-end" enclose a fold; folds are not nested.
// Invalid markers (end without start, start without end, nested start) mean: no folds.

const START_MARKER = '// @fold-start';
const END_MARKER = '// @fold-end';

/**
 * Splits code into visible parts and folds, in file order. Marker lines are removed.
 * Returns {parts: Array<{kind: 'visible'|'fold', code, lineCount, indent}>, hasFolds}.
 */
export function parseFolds(code) {
  const text = code.replace(/\r\n/g, '\n');
  const unchanged = {
    parts: [{kind: 'visible', code: text, lineCount: text.split('\n').length, indent: ''}],
    hasFolds: false,
  };
  const parts = [];
  let current = []; // lines of the visible part being collected
  let block = null; // lines of the open fold, null = none
  let indent = '';

  function flushVisible() {
    if (current.length > 0) {
      parts.push({kind: 'visible', code: current.join('\n'), lineCount: current.length, indent: ''});
      current = [];
    }
  }

  for (const line of text.split('\n')) {
    const trimmed = line.trim();
    if (trimmed === START_MARKER) {
      if (block !== null) { return unchanged; } // nested start
      flushVisible();
      block = [];
      indent = line.slice(0, line.length - line.trimStart().length);
    } else if (trimmed === END_MARKER) {
      if (block === null) { return unchanged; } // end without start
      parts.push({kind: 'fold', code: block.join('\n'), lineCount: block.length, indent});
      block = null;
    } else if (block !== null) {
      block.push(line);
    } else {
      current.push(line);
    }
  }
  if (block !== null) { return unchanged; } // start without end
  flushVisible();

  const hasFolds = parts.some((part) => part.kind === 'fold');
  return hasFolds ? {parts, hasFolds} : unchanged;
}

// Same text as the runner placeholder in playground/CodeFolds.cs.
export function placeholder(indent, k, n) {
  return `${indent}// ⋯ Block ${k}: ${n} Zeilen ausgeblendet ⋯`;
}

/** The code as the runner shows it: visible parts, one placeholder line per fold. */
export function visibleCode(code) {
  const {parts, hasFolds} = parseFolds(code);
  if (!hasFolds) { return parts[0].code; }
  let k = 0;
  return parts
    .map((part) => (part.kind === 'fold' ? placeholder(part.indent, ++k, part.lineCount) : part.code))
    .join('\n');
}
