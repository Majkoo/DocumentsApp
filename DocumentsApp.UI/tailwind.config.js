import flowbite from "flowbite/plugin.js";

/** @type {import('tailwindcss').Config} */
const theme = {
  'prim': {
    '50': '#faf5ff',
    '100': '#f3e8ff',
    '200': '#e9d5ff',
    '300': '#d8b4fe',
    '400': '#c084fc',
    '500': '#a855f7',
    '600': '#9333ea',
    '700': '#7e22ce',
    '800': '#6b21a8',
    '900': '#581c87',
    '950': '#3b0764',
  },
  'accent': {
    '50': '#f0fafa',
    '100': '#dff2f2',
    '200': '#b4e0e0',
    '300': '#8ccfcd',
    '400': '#4daba9',
    '500': '#1c8786',
    '600': '#177a74',
    '700': '#10665b',
    '800': '#0b5242',
    '900': '#063d2d',
    '950': '#022619'
  },
  'danger': {
    '50': '#fffaf5',
    '100': '#fff4eb',
    '200': '#ffe1cf',
    '300': '#ffc8b0',
    '400': '#ff8873',
    '500': '#ff3838',
    '600': '#e62e2e',
    '700': '#bf1f1f',
    '800': '#991414',
    '900': '#730b0b',
    '950': '#4a0404'
  },
  'warn': {
    '50': '#fffff5',
    '100': '#ffffeb',
    '200': '#feffcf',
    '300': '#fbfcb1',
    '400': '#fafc77',
    '500': '#f8fb3c',
    '600': '#d5e031',
    '700': '#a6ba22',
    '800': '#7a9615',
    '900': '#54700c',
    '950': '#2f4705'
  },
}

export default {
  content: [
    './src/**/*.{html,js,svelte,ts}',
    './node_modules/flowbite-svelte/**/*.{html,js,svelte,ts}'
  ],
  darkMode: 'class',
  mode: 'jit',
  theme: {
    extend: {
      colors: theme
    }
  },
  plugins: [
    flowbite
  ]
};