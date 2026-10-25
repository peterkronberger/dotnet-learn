import React from 'react';

// Renders text with <mark> around each [start, end) range. Ranges must be sorted and not overlap.
export default function Highlight({text, ranges}) {
  if (!ranges || ranges.length === 0) return <>{text}</>;
  const parts = [];
  let pos = 0;
  ranges.forEach(([start, end], i) => {
    if (start > pos) parts.push(text.slice(pos, start));
    parts.push(<mark key={i}>{text.slice(start, end)}</mark>);
    pos = end;
  });
  if (pos < text.length) parts.push(text.slice(pos));
  return <>{parts}</>;
}
