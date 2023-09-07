<script lang="ts">
    import { page } from '$app/stores';
    import Nav from "$lib/layout/Nav.svelte";
    import "../../app.css"
    import {onMount} from "svelte";
    import {initFlowbite} from "flowbite";

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