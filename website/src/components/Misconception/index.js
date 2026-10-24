import React from 'react';

// Invisible anchor; place it as the first line inside a `:::warning[Achtung]` box.
export default function Misconception({id}) {
  return <span id={`misconception-${id}`} data-misconception={id} />;
}
