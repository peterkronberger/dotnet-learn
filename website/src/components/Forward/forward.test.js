const test = require('node:test');
const assert = require('node:assert');
const {validateForwardProps, conceptParts} = require('./forward');

const ids = new Set(['m00-l01', 'm00-l05']);

test('valid props pass', () => {
  validateForwardProps({concepts: ['a'], lesson: 'm00-l01'}, ids);
  validateForwardProps({concepts: ['a', 'b'], module: '6'}, ids);
  validateForwardProps({concepts: ['a'], module: 0}, ids);
});

test('invalid props throw a <Forward> error', () => {
  const bad = [
    {},
    {concepts: [], lesson: 'm00-l01'},
    {concepts: 'a', lesson: 'm00-l01'},
    {concepts: [''], lesson: 'm00-l01'},
    {concepts: [1], lesson: 'm00-l01'},
    {concepts: ['a'], lesson: 'm00-l01', module: '1'},
    {concepts: ['a']},
    {concepts: ['a'], lesson: 'm99-l01'},
    {concepts: ['a'], lesson: 'm01-l03'},
    {concepts: ['a'], module: '20'},
    {concepts: ['a'], module: 'x'},
    {concepts: ['a'], module: 1.5},
    {concepts: ['a'], module: '-1'},
  ];
  for (const props of bad) {
    assert.throws(() => validateForwardProps(props, ids), /^Error: <Forward/, JSON.stringify(props));
  }
});

test('early-implementation lessons are rejected even if they exist', () => {
  const all = new Set([...ids, 'm01-l02', 'm01-l03', 'm02-l01']);
  for (const id of ['m01-l02', 'm01-l03', 'm02-l01']) {
    assert.throws(() => validateForwardProps({concepts: ['a'], lesson: id}, all), /module="N"/);
  }
});

test('conceptParts', () => {
  assert.deepStrictEqual(conceptParts('Variablen'), [{text: 'Variablen', code: false}]);
  assert.deepStrictEqual(conceptParts('`var`'), [{text: 'var', code: true}]);
  assert.deepStrictEqual(conceptParts('Methode `Main`'), [
    {text: 'Methode ', code: false},
    {text: 'Main', code: true},
  ]);
  assert.deepStrictEqual(conceptParts('a `b` c'), [
    {text: 'a ', code: false},
    {text: 'b', code: true},
    {text: ' c', code: false},
  ]);
});
