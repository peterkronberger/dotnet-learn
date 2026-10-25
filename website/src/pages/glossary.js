import React from 'react';
import Layout from '@theme/Layout';
import Translate, {translate} from '@docusaurus/Translate';
import Glossary from '@site/src/components/Glossary';

export default function GlossaryPage() {
  return (
    <Layout
      title={translate({id: 'glossary.page.title', message: 'Glossar'})}
      description={translate({
        id: 'glossary.page.description',
        message: 'Fachbegriffe aus dem Kurs mit kurzer Erklärung, Quelle und Verweis auf die Lektion.',
      })}>
      <main className="container margin-vert--lg">
        <h1>
          <Translate id="glossary.page.heading">Glossar</Translate>
        </h1>
        <Glossary />
      </main>
    </Layout>
  );
}
