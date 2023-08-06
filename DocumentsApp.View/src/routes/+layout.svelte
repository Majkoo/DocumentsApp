<script>
	import Sidenav from "$lib/layout/Sidenav.svelte";
	import Header from "$lib/layout/Header.svelte";
</script>

<div class="d-flex layout">

	<Sidenav />

	<div class="w-100 d-flex flex-column">
		<Header />

		<main class="p-3 flex-fill">
			<slot />
		</main>
	</div>

</div>

<style global>
	@import "../app.scss";

	.layout {
        height: 100vh;
		flex-grow: 1;
	}
</style>
