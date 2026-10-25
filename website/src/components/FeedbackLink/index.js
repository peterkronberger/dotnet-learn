import React from 'react';
import Translate from '@docusaurus/Translate';
import useDocusaurusContext from '@docusaurus/useDocusaurusContext';
import {useDoc} from '@docusaurus/plugin-content-docs/client';
import styles from './styles.module.css';

// Builds the "new issue" URL for the feedback issue form (.github/ISSUE_TEMPLATE/feedback.yml).
// The form sets its labels itself, so no `labels` parameter is added here.
export function buildFeedbackUrl({repoUrl, pageUrl, title, lessonId}) {
  const params = new URLSearchParams();
  params.append('template', 'feedback.yml');
  params.append('title', `[Feedback] ${title}`);
  params.append('page-url', pageUrl);
  if (typeof lessonId === 'string' && lessonId !== '') {
    params.append('lesson-id', lessonId);
  }
  return `${repoUrl}/issues/new?${params.toString()}`;
}

// Link at the end of every docs page. It only opens GitHub; the site sends nothing itself.
export default function FeedbackLink() {
  const {siteConfig} = useDocusaurusContext();
  const {metadata, frontMatter} = useDoc();
  const repoUrl = `https://github.com/${siteConfig.organizationName}/${siteConfig.projectName}`;
  const url = buildFeedbackUrl({
    repoUrl,
    pageUrl: siteConfig.url.replace(/\/$/, '') + metadata.permalink,
    title: metadata.title,
    lessonId: frontMatter.lesson_id,
  });
  return (
    <div className={styles.box}>
      <p>
        <Translate id="feedback.hint">
          Hast du einen Fehler gefunden oder eine Idee? Dafür brauchst du ein GitHub-Konto. Das Issue ist öffentlich.
        </Translate>
      </p>
      <a href={url} target="_blank" rel="noopener noreferrer">
        <Translate id="feedback.link">Fehler melden / Feedback</Translate>
        <span className={styles.visuallyHidden}>
          {' '}
          <Translate id="feedback.newTab">(öffnet GitHub in einem neuen Tab)</Translate>
        </span>
        <span aria-hidden="true"> ↗</span>
      </a>
    </div>
  );
}
