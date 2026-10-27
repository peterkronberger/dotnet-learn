import {useCallback, useLayoutEffect, useRef, useState} from 'react';

// Random order of options without hydration problems:
// - The server render and the first client render use the identity order (data order),
//   so both produce the same HTML.
// - A layout effect shuffles once after mount, before the browser paints.
// - Until that first shuffle ran, `ready` is false. Components give the shuffled list the CSS
//   class `shuffling` (`visibility: hidden`): the space stays reserved, so nothing jumps.
// Components keep `order` as a list of original indices and keep all state in original indices.

// Fisher-Yates. Returns a new array; does not change `items`.
export function shuffle(items, random = Math.random) {
  const result = [...items];
  for (let i = result.length - 1; i > 0; i--) {
    const j = Math.floor(random() * (i + 1));
    [result[i], result[j]] = [result[j], result[i]];
  }
  return result;
}

// Returns a permutation of 0..length-1 (original indices).
// Indices in `pinnedLast` stay at the end in ascending order; the others are shuffled.
// If `previous` is given, more than two indices are shuffled, and the result equals `previous`,
// draw once more (the second result is used even if it is equal again).
export function newOrder(length, {previous, pinnedLast = [], random = Math.random} = {}) {
  const pinned = [...pinnedLast].sort((a, b) => a - b);
  const free = Array.from({length}, (_, i) => i).filter((i) => !pinned.includes(i));
  const draw = () => [...shuffle(free, random), ...pinned];
  let result = draw();
  const same = (a, b) => Array.isArray(b) && a.length === b.length && a.every((v, i) => v === b[i]);
  if (previous && free.length > 2 && same(result, previous)) result = draw();
  return result;
}

// Returns {order, ready, reshuffle}.
export function useShuffledOrder(length, {pinnedLast = []} = {}) {
  const [order, setOrder] = useState(() => Array.from({length}, (_, i) => i));
  const [ready, setReady] = useState(false);
  // `pinnedLast` is a new array on every render, so it is only read through a ref.
  const pinnedRef = useRef(pinnedLast);
  pinnedRef.current = pinnedLast;
  const orderRef = useRef(order);
  orderRef.current = order;

  const reshuffle = useCallback(() => {
    setOrder(newOrder(length, {previous: orderRef.current, pinnedLast: pinnedRef.current}));
  }, [length]);

  useLayoutEffect(() => {
    reshuffle();
    setReady(true);
  }, [reshuffle]);

  return {order, ready, reshuffle};
}
