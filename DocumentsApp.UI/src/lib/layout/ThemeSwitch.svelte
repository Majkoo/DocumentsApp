<script lang="ts">
    import { browser } from '$app/environment';

    let darkMode = true;

    function handleSwitchDarkMode() {
        darkMode = !darkMode;

        localStorage.setItem('theme', darkMode ? 'dark' : 'light');

        darkMode
            ? document.documentElement.classList.add('dark')
            : document.documentElement.classList.remove('dark');
    }

    if (browser) {
        if (
            localStorage.theme === 'dark' ||
            (!('theme' in localStorage) && window.matchMedia('(prefers-color-scheme: dark)').matches)
        ) {
            document.documentElement.classList.add('dark');
            darkMode = true;
        } else {
            document.documentElement.classList.remove('dark');
            darkMode = false;
        }
    }
</script>

<div class="header-link" aria-roledescription="dark mode switch" on:click={() => handleSwitchDarkMode()}>
	<input checked={darkMode} on:click={handleSwitchDarkMode} type="checkbox" id="theme-toggle" />
	<span class="material-icons-outlined">
		{#if darkMode}light_mode{/if}
		{#if !darkMode}dark_mode{/if}
	</span>
</div>

<style lang="postcss">

	input {
		display: none;
	}
	label {
		height: min-content !important;
	}

</style>