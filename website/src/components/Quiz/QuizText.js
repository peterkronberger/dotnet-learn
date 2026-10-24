import React from 'react';

// Inline code: `x` becomes <code>.
export function renderInline(text) {
  return String(text)
    .split('`')
    .map((part, index) =>
      index % 2 === 1 ? <code key={index}>{part}</code> : <span key={index}>{part}</span>,
    );
}

// Paragraphs are separated by a blank line; inline code as in renderInline.
export default function QuizText({text}) {
  return String(text)
    .split('\n\n')
    .map((paragraph, index) => <p key={index}>{renderInline(paragraph)}</p>);
}
