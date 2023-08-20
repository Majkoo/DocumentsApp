<script lang="ts">
	import "../../app.css"
    import { page } from '$app/stores';
    import {onMount} from "svelte";
    import {initFlowbite} from "flowbite";
    import Nav from "$lib/layout/Nav.svelte";

    onMount(() => {
        initFlowbite();
	})

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

<style global>
    @import "../../../node_modules/material-icons";
</style>