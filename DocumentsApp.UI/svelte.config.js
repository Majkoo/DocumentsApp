import adapter from '@sveltejs/adapter-auto';
import autoPreprocess from 'svelte-preprocess';
const config = {
	kit: {
		adapter: adapter()
	},
	preprocess: autoPreprocess()
};
export default config;