<script lang="ts">
	import "../../app.css"
    import { page } from '$app/stores';
    import ThemeSwitch from "$lib/layout/ThemeSwitch.svelte";
    import Header from "$lib/layout/Header.svelte";

    $: currentRoute = $page.url.pathname;
</script>

<div class="flex flex-col h-full">

	<Header bind:currentRoute={currentRoute}/>

	<main>

		<aside>
			<div class="flex-grow">

				<a class="nav-link" href="/my-items" class:active={currentRoute === '/my-items'}>
					My items
					<span class="material-icons-outlined">folder</span>
				</a>

				<a class="nav-link" href="/create" class:active={currentRoute === '/create'}>
					Create
					<span class="material-icons-outlined">add</span>
				</a>

				<a class="nav-link" href="/share" class:active={currentRoute ===  '/share'}>
					Share
					<span class="material-icons-outlined">share</span>
				</a>

			</div>

			<a class="nav-link text-gray-700 text-opacity-50" href="/help" class:active={currentRoute ===  '/help'}>
				<span class="w-full">Need some help</span>
				<span class="material-icons-outlined">question_mark</span>
			</a>

		</aside>

		<section class="content">

			<slot />

		</section>

	</main>

</div>


<style global>
    @import "../../../node_modules/material-icons";
</style>