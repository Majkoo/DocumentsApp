<script lang="ts">
	import ThemeSwitch from "$lib/layout/ThemeSwitch.svelte";
    export let currentRoute: string;
</script>

<header>
	<a class="title" href="/">
		<span class="material-icons-outlined">description</span>
		DocumentsApp
	</a>

	<div class="flex">
		<a class="header-link" href="/account" class:active={currentRoute === '/account'}>
			<span class="material-icons-outlined">person</span>
		</a>
		<ThemeSwitch />
	</div>
</header>

<style lang="postcss">

</style>