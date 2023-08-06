<script>
	import Header from "$lib/layout/Header.svelte";
    import DocNav from "$lib/layout/DocNav.svelte";
</script>


<div class="body d-flex flex-column">

	<Header />

	<main class="d-flex flex-column flex-lg-row">
		<DocNav />

		<div class="page-content">
			<slot />
		</div>
	</main>


</div>

<style global>
	@import "../../app.scss";
	.body {
        height: 100vh;
	}
	main {
		flex-grow: 1;
	}
</style>
