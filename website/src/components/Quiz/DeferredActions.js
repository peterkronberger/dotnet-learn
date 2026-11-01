import React, {useEffect, useRef} from 'react';
import Translate from '@docusaurus/Translate';
import styles from './styles.module.css';

// Buttons and status text of the deferred mode (the answer is saved, but not revealed).
export default function DeferredActions({saved, canSave, onEdit}) {
  const editButton = useRef(null);

  // After saving, the focus moves to the "change" button (it replaces the focused submit button).
  useEffect(() => {
    if (saved && editButton.current) editButton.current.focus();
  }, [saved]);

  if (!saved) {
    return (
      <div className={styles.actions}>
        <button type="submit" className="button button--primary" disabled={!canSave}>
          <Translate id="quiz.deferred.save">Antwort speichern</Translate>
        </button>
      </div>
    );
  }
  return (
    <div className={styles.actions}>
      <p role="status" className={styles.deferredStatus}>
        <Translate id="quiz.deferred.saved">Antwort gespeichert.</Translate>
      </p>
      <button type="button" className="button button--secondary" ref={editButton} onClick={onEdit}>
        <Translate id="quiz.deferred.edit">Antwort ändern</Translate>
      </button>
    </div>
  );
}

// After "Antwort ändern" the focus moves to the first input of the form.
// Usage: const {formRef, requestFocus} = useEditFocus(); call requestFocus() in edit().
export function useEditFocus() {
  const formRef = useRef(null);
  const pending = useRef(false);
  useEffect(() => {
    if (!pending.current) return;
    pending.current = false;
    const target = formRef.current
      ? formRef.current.querySelector('input:not(:disabled), select:not(:disabled), textarea, button:not(:disabled)')
      : null;
    if (target) target.focus();
  });
  return {
    formRef,
    requestFocus() {
      pending.current = true;
    },
  };
}
