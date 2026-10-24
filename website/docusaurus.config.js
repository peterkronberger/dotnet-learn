// @ts-check
const {themes: prismThemes} = require('prism-react-renderer');

// GitHub Pages (Projektseite): https://peterkronberger.github.io/dotnet-learn/
const baseUrl = '/dotnet-learn/';

/** @type {import('@docusaurus/types').Config} */
const config = {
  title: '.NET lernen',
  tagline: 'Verstehen, vorhersagen, ausprobieren – C# direkt im Browser',
  favicon: 'img/favicon.svg',

  url: 'https://peterkronberger.github.io',
  baseUrl,
  organizationName: 'peterkronberger',
  projectName: 'dotnet-learn',
  // GitHub Pages adds trailing slashes by default; the Docusaurus docs recommend
  // setting trailingSlash explicitly (true or false).
  trailingSlash: false,

  onBrokenLinks: 'warn',

  i18n: {
    defaultLocale: 'de',
    locales: ['de'],
  },

  presets: [
    [
      'classic',
      /** @type {import('@docusaurus/preset-classic').Options} */
      ({
        docs: {
          sidebarPath: require.resolve('./sidebars.js'),
          routeBasePath: 'docs',
          editUrl: 'https://github.com/peterkronberger/dotnet-learn/edit/main/website/',
        },
        blog: false,
        theme: {
          customCss: './src/css/custom.css',
        },
      }),
    ],
  ],

  // Lesson index (title, duration, learning goals, module) for the Lernstand page.
  plugins: [require.resolve('./plugins/lesson-index.js')],

  themeConfig:
    /** @type {import('@docusaurus/preset-classic').ThemeConfig} */
    ({
      colorMode: {
        defaultMode: 'light',
        respectPrefersColorScheme: true,
      },
      navbar: {
        title: '.NET lernen',
        logo: {alt: 'Logo .NET lernen', src: 'img/logo.svg'},
        items: [
          {
            type: 'docSidebar',
            sidebarId: 'tutorialSidebar',
            position: 'left',
            label: 'Lernpfad',
          },
          {to: '/playground', label: 'Playground', position: 'left'},
          {to: '/dashboard', label: 'Lernstand', position: 'left'},
          {
            href: 'https://github.com/peterkronberger/dotnet-learn',
            label: 'GitHub',
            position: 'right',
          },
        ],
      },
      footer: {
        style: 'light',
        copyright:
          'Lernseite für C# und .NET · Quellcode auf GitHub · Der Playground kompiliert C# lokal in deinem Browser.',
      },
      docs: {
        sidebar: {hideable: true, autoCollapseCategories: false},
      },
      prism: {
        theme: prismThemes.github,
        darkTheme: prismThemes.dracula,
        additionalLanguages: ['csharp'],
      },
    }),
};

module.exports = config;
