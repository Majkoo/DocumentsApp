<script lang="ts">
    import {onMount} from "svelte";

    let darkMode: boolean;

    onMount(() => {
        darkMode = localStorage.getItem('theme') == 'dark';
    })

    function handleSwitchDarkMode() {

        darkMode = !darkMode;

        localStorage.setItem('theme', darkMode ? 'dark' : 'light');

        darkMode
            ? document.documentElement.classList.add('dark')
            : document.documentElement.classList.remove('dark');
    }
    export let expand = false;
</script>

<button class="{expand ? 'w-full' : ''} nav-link"  aria-roledescription="dark mode switch" on:click={() => handleSwitchDarkMode()}>
	<input checked={darkMode} on:click={handleSwitchDarkMode} type="checkbox" id="theme-toggle" />
	<span class="whitespace-nowrap text-left {expand ? 'nav-link-expand' : 'hidden'}">
		{#if darkMode}Light mode{/if}
		{#if !darkMode}Dark mode{/if}
	</span>
	<span class="material-icons-outlined">
		{#if darkMode}light_mode{/if}
		{#if !darkMode}dark_mode{/if}
	</span>
</button>

<style lang="postcss">

	input {
		display: none;
	}
	label {
		height: min-content !important;
	}

</style>