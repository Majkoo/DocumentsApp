import { sveltekit } from '@sveltejs/kit/vite';
import { defineConfig } from 'vitest/config';

export default defineConfig({
	plugins: [sveltekit()],
<<<<<<< HEAD

	test: {
		include: ['src/**/*.{test,spec}.{js,ts}']
	},

	css: {
		preprocessorOptions: {
			scss: {
				additionalData: '@use "src/variables.scss" as *;'
			}
		}
=======
	test: {
		include: ['src/**/*.{test,spec}.{js,ts}']
>>>>>>> 591a508a94d5c9904127f675d711585de01ff33b
	}
});
