import React from 'react';
import Link from '@docusaurus/Link';

// Single list of all developer showcase pages. Add new pages here.
export const DEV_PAGES = [
  { to: '/dev/quiz-demo', title: 'Quiz-Typen', description: 'Je eine Frage pro Quiz-Typ, mit Misconception-Box.' },
  {
    to: '/dev/runner-demo',
    title: 'CSharpRunner',
    description: 'Vorhersagefeld, geprüfte Aufgabe (Tests) und Schrittansicht (trace).',
  },
  {
    to: '/dev/components-demo',
    title: 'Since und FromLanguage',
    description: 'Versions-Badge und Umsteiger-Box.',
  },
  {
    to: '/dev/components-demo-2',
    title: 'FromLanguage (Seite 2)',
    description: 'Prüft, dass die Sprachwahl seitenübergreifend gilt.',
  },
  {
    to: '/dev/diagram-demo',
    title: 'Diagramme (Mermaid)',
    description: 'Mermaid-Diagramme mit Textbeschreibung, hell und dunkel.',
  },
  {
    to: '/dev/progress-selftest',
    title: 'Selbsttest: Lernfortschritt',
    description: 'Führt die Selbsttests der Fortschrittsbibliothek im Browser aus.',
  },
  {
    to: '/dev/search-selftest',
    title: 'Selbsttest: Suche',
    description: 'Führt die Selbsttests der Suchlogik im Browser aus.',
  },
];

// Navigation across all dev pages; the overview page shows descriptions as well.
export default function DevNav({ withDescriptions = false }) {
  return (
    <nav aria-label="Entwicklerseiten">
      <ul>
        <li>
          <Link to="/dev/">Übersicht</Link>
        </li>
        {DEV_PAGES.map((page) => (
          <li key={page.to}>
            <Link to={page.to}>{page.title}</Link>
            {withDescriptions && <> – {page.description}</>}
          </li>
        ))}
      </ul>
    </nav>
  );
}
