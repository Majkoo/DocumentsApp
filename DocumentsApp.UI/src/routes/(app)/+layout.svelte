<script lang="ts">
    import { page } from '$app/stores';
    import Nav from "$lib/components/layout/Nav.svelte";
    import "../../app.css"

    $: currentRoute = $page.url.pathname;
</script>

<div class="flex flex-col h-full">

	<main>

		<Nav currentRoute="{currentRoute}"/>

		<section class="content">

			<slot />

		</section>

	</main>

</div>