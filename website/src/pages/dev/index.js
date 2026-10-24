import React from 'react';
import Layout from '@theme/Layout';
import Head from '@docusaurus/Head';
import DevNav from '@site/src/components/DevNav';

// Entry point for all developer showcase pages.
export default function DevIndex() {
  return (
    <Layout title="Entwicklerseiten">
      <Head>
        <meta name="robots" content="noindex" />
      </Head>
      <main className="container margin-vert--lg">
        <h1>Entwicklerseiten</h1>
        <p>Showcase aller Komponenten. Keine Lerninhalte.</p>
        <DevNav withDescriptions />
      </main>
    </Layout>
  );
}
