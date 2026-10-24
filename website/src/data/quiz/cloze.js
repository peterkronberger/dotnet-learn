// Splits example code into code parts and blanks. Each blank is the first unused occurrence of
// blanks[i].text, searched from the end of the previous blank. Throws if a text is not found.
// Keep in sync with the copy in tools/check-quiz.mjs.
export function splitCloze(code, blanks) {
  const parts = [];
  let position = 0;
  blanks.forEach((blank, index) => {
    const found = blank.text ? code.indexOf(blank.text, position) : -1;
    if (found < 0) {
      throw new Error(`Cloze blank ${index + 1} "${blank.text}" not found in the example code`);
    }
    if (found > position) parts.push({kind: 'code', text: code.slice(position, found)});
    parts.push({kind: 'blank', index});
    position = found + blank.text.length;
  });
  if (position < code.length) parts.push({kind: 'code', text: code.slice(position)});
  return parts;
}

export const normalizeBlank = (text) => String(text).replace(/\s+/g, '');

// Example code as shown in the cloze: line endings unified, trailing newlines removed.
export const clozeCode = (code) => String(code).replace(/\r\n?/g, '\n').replace(/\n+$/, '');
