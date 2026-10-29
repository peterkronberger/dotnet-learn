// Pure logic of the <Forward> component (no React), so it can be tested with `node --test`.
const EARLY_IMPLEMENTATIONS = ['m02-l01'];

const MIN_MODULE = 0;
const MAX_MODULE = 19;

function isSet(value) {
  return value !== undefined && value !== null && value !== '';
}

// Throws an Error with a message that starts with "<Forward>:" when the props are invalid.
// lessonIds: Set<string> of all lesson_id values.
function validateForwardProps({concepts, lesson, module}, lessonIds) {
  if (!Array.isArray(concepts) || concepts.length === 0) {
    throw new Error('<Forward>: "concepts" must be a non-empty array of strings.');
  }
  if (concepts.some((c) => typeof c !== 'string' || c.trim() === '')) {
    throw new Error('<Forward>: every entry of "concepts" must be a non-empty string.');
  }
  const hasLesson = isSet(lesson);
  const hasModule = isSet(module);
  if (hasLesson === hasModule) {
    throw new Error('<Forward>: set exactly one of "lesson" or "module".');
  }
  if (hasLesson) {
    if (EARLY_IMPLEMENTATIONS.includes(lesson)) {
      throw new Error(
        `<Forward lesson="${lesson}">: this is an early-implementation lesson with an unstable id; use module="N" instead.`,
      );
    }
    if (!lessonIds.has(lesson)) {
      throw new Error(
        `<Forward lesson="${lesson}">: unknown lesson_id. Use an existing lesson or module="N".`,
      );
    }
    return;
  }
  const isNumberLike =
    (typeof module === 'number' && Number.isInteger(module)) ||
    (typeof module === 'string' && /^\d+$/.test(module.trim()));
  const n = isNumberLike ? Number(module) : NaN;
  if (!(n >= MIN_MODULE && n <= MAX_MODULE)) {
    throw new Error(
      `<Forward module="${module}">: "module" must be an integer from ${MIN_MODULE} to ${MAX_MODULE}.`,
    );
  }
}

// Splits one concept name into parts for rendering: [{text, code: boolean}].
// 'Methode `Main`' -> [{text: 'Methode ', code: false}, {text: 'Main', code: true}]
function conceptParts(name) {
  return name
    .split('`')
    .map((text, i) => ({text, code: i % 2 === 1}))
    .filter((part) => part.text !== '');
}

module.exports = {EARLY_IMPLEMENTATIONS, validateForwardProps, conceptParts};
