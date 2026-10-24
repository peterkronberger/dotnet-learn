import React, {useId, useState} from 'react';
import clsx from 'clsx';
import Translate, {translate} from '@docusaurus/Translate';
import {parseImport, serializeProgress} from '@site/src/lib/progress/exportImport';
import {MAX_IMPORT_LENGTH} from '@site/src/lib/progress/config';
import {toLocalDate} from '@site/src/lib/progress/dates';
import styles from './styles.module.css';

// Export/import of the learning progress as a JSON file; no network access.
export default function ProgressTransfer({progress, replace}) {
  const inputId = useId();
  const [error, setError] = useState(null);
  const [pending, setPending] = useState(null);
  const [message, setMessage] = useState('');

  function exportProgress() {
    const blob = new Blob([serializeProgress(progress)], {type: 'application/json'});
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = `dotnet-learn-progress-${toLocalDate()}.json`;
    document.body.appendChild(a);
    a.click();
    a.remove();
    URL.revokeObjectURL(url);
    setError(null);
    setPending(null);
    setMessage(translate({id: 'dashboard.exported', message: 'Der Lernstand wurde exportiert.'}));
  }

  async function onFile(event) {
    const input = event.target;
    const file = input.files && input.files[0];
    input.value = '';
    setMessage('');
    setPending(null);
    setError(null);
    if (!file) return;
    if (file.size > MAX_IMPORT_LENGTH) {
      setError(translate({id: 'dashboard.import.tooLarge', message: 'Die Datei ist zu groß (maximal 1 MB).'}));
      return;
    }
    let text;
    try {
      text = await file.text();
    } catch {
      setError(translate({id: 'dashboard.import.unreadable', message: 'Die Datei konnte nicht gelesen werden.'}));
      return;
    }
    const result = parseImport(text);
    if (!result.ok) {
      setError(result.error);
      return;
    }
    setPending(result.progress);
  }

  function confirmReplace() {
    replace(pending);
    setPending(null);
    setMessage(translate({id: 'dashboard.import.done', message: 'Der Lernstand wurde importiert.'}));
  }

  return (
    <div className={clsx(styles.card, styles.dataCard)}>
      <p className={styles.dataText}>
        <Translate id="lernstand.data.text">
          Dein Lernstand liegt nur in diesem Browser, ohne Konto und ohne Tracking. Mit dem Export sicherst du ihn als Datei oder überträgst ihn auf ein anderes Gerät.
        </Translate>
      </p>
      <div className={styles.dataActions}>
        <button type="button" className={styles.buttonSecondary} onClick={exportProgress}>
          <Translate id="dashboard.export">Lernstand exportieren</Translate>
        </button>
        <input
          id={inputId}
          className={styles.fileInput}
          type="file"
          accept="application/json,.json"
          onChange={onFile}
        />
        <label htmlFor={inputId} className={clsx(styles.buttonSecondary, styles.fileLabel)}>
          <Translate id="dashboard.import">Lernstand importieren</Translate>
        </label>
      </div>
      {pending && (
        <div className={styles.notice}>
          <p>
            <Translate
              id="dashboard.import.confirm"
              values={{n: Object.keys(pending.questions).length, m: Object.keys(progress.questions).length}}>
              {'Die Datei enthält {n} Fragen. Dein aktueller Lernstand ({m} Fragen, abgeschlossene Lektionen und Aktivität) wird ersetzt.'}
            </Translate>
          </p>
          <div className={styles.actions}>
            <button type="button" className={styles.buttonPrimary} onClick={confirmReplace}>
              <Translate id="dashboard.import.replace">Ersetzen</Translate>
            </button>
            <button type="button" className={styles.buttonSecondary} onClick={() => setPending(null)}>
              <Translate id="dashboard.import.cancel">Abbrechen</Translate>
            </button>
          </div>
        </div>
      )}
      <div role="alert" className={styles.dataMessages}>
        {error && <p className={styles.error}>{error}</p>}
      </div>
      <div aria-live="polite" className={styles.dataMessages}>
        {message && <p className={styles.success}>{message}</p>}
      </div>
    </div>
  );
}
