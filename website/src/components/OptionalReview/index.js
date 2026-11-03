import React from 'react';
import Translate from '@docusaurus/Translate';
import Admonition from '@theme/Admonition';
import useProgress from '@site/src/lib/progress/useProgress';
import {allPlacementsPassed} from '@site/src/lib/progress/placement';

function joinList(numbers) {
  if (numbers.length === 1) return String(numbers[0]);
  return `${numbers.slice(0, -1).join(', ')} und ${numbers[numbers.length - 1]}`;
}

/**
 * Note for review lessons. Props: modules (non-empty list of module numbers 0-19).
 * Shown only if the placement test of every listed module is passed.
 * Read-only: nothing is written to the progress store.
 */
export default function OptionalReview({modules}) {
  const valid =
    Array.isArray(modules) &&
    modules.length > 0 &&
    modules.every((n) => Number.isInteger(n) && n >= 0 && n <= 19);
  if (!valid) {
    throw new Error('<OptionalReview>: "modules" must be a non-empty list of module numbers (0–19).');
  }
  const {progress} = useProgress();
  const keys = modules.map((n) => 'm' + String(n).padStart(2, '0'));
  // progress is null before the browser storage is read: the server render stays empty.
  if (progress === null || !allPlacementsPassed(progress, keys)) return null;

  return (
    <Admonition
      type="info"
      title={<Translate id="optionalReview.title">Wiederholung ist optional</Translate>}>
      <p>
        <Translate id="optionalReview.text" values={{list: joinList(modules)}}>
          {'Du hast die Einstufungstests der Module {list} bestanden. Diese Wiederholung ist für dich optional: Du kannst sie bearbeiten oder direkt mit dem nächsten Modul weitermachen.'}
        </Translate>
      </p>
    </Admonition>
  );
}
