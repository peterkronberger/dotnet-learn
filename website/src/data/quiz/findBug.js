// Lines of an example as shown in a `find-bug` question: line endings unified, trailing newlines
// removed. Line numbers are 1-based like in the file. Keep in sync with tools/check-quiz.mjs.
export const findBugLines = (code) => String(code).replace(/\r\n?/g, '\n').replace(/\n+$/, '').split('\n');
