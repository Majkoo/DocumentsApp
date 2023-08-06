<nav class="aside d-flex flex-column text-white">

	<div class="h-100 d-flex flex-column">
		<div class="side-title w-100">
			<a
					href="/"
					class="w-100 nav-link p-0 justify-content-start">
				<i class="bi bi-image nav-icon"></i>
				<h3 class="m-0">DocApp</h3>
			</a>
		</div>

		<div class="d-flex flex-column side-menu">
			<a href="/" class="nav-link">
				Documents
				<i class="bi bi-folder2 nav-icon"></i>
			</a>
			<a href="/create" class="nav-link">
				Create
				<i class="bi bi-plus-circle nav-icon"></i>
			</a>
			<a href="/share" class="nav-link">
				Share
				<i class="bi bi-share nav-icon"></i>
			</a>
		</div>

		<div class="d-flex flex-column justify-content-end side-menu flex-fill">
			<a href="/help" class="nav-link help-link justify-content-between align-items-center">
				<span class="flex-nowrap">Need some help</span>
				<i class="bi bi-question-circle"></i>
			</a>
		</div>
	</div>

</nav>

<style lang="scss">
	.help-link {
		opacity: 0.6;
		font-size: 1rem;
		i {
			font-size: 1.25rem;
		}
	  	&:hover {
		  opacity: 0.5;
		}
	}
</style>
