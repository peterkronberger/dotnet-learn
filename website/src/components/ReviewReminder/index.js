import React, {useState} from 'react';
import Link from '@docusaurus/Link';
import Translate from '@docusaurus/Translate';
import Admonition from '@theme/Admonition';
import useProgress from '@site/src/lib/progress/useProgress';
import {toLocalDate} from '@site/src/lib/progress/dates';
import {reviewReminder} from '@site/src/lib/progress/leitner';
import useProgressDashboardData from '@site/src/components/ProgressDashboard/useProgressDashboardData';

/**
 * Reminder box at the top of lesson pages. No props.
 * Rule (see `reviewReminder` in lib/progress/leitner.js): questions are due and the last review
 * is more than REMINDER_AFTER_DAYS days ago. Read-only: nothing is written to the store.
 * Renders nothing on the server render and before the browser storage is read.
 */
export default function ReviewReminder() {
  const {progress} = useProgress();
  const [today] = useState(() => toLocalDate());
  const data = useProgressDashboardData(progress, today);
  if (!data) return null;
  const r = reviewReminder(progress, data.due, today);
  if (!r.show) return null;

  return (
    <Admonition
      type="info"
      title={<Translate id="reviewReminder.title">Zeit für eine Wiederholung</Translate>}>
      <p>
        {r.dueCount === 1 ? (
          <Translate id="reviewReminder.text.one">
            {'1 Quizfrage ist zur Wiederholung fällig. Wiederhole sie im Lernstand, dann bleibt das Gelernte besser hängen.'}
          </Translate>
        ) : (
          <Translate id="reviewReminder.text.other" values={{count: r.dueCount}}>
            {'{count} Quizfragen sind zur Wiederholung fällig. Wiederhole sie im Lernstand, dann bleibt das Gelernte besser hängen.'}
          </Translate>
        )}
      </p>
      <p>
        <Link to="/dashboard#heute">
          <Translate id="reviewReminder.link">Zum Lernstand und wiederholen</Translate>
        </Link>
      </p>
    </Admonition>
  );
}
