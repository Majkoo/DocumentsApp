<nav class="doc-nav d-flex flex-row text-white">

	<div class="h-100 w-100 d-flex flex-row flex-lg-column">

		<div class="d-flex flex-row flex-lg-column justify-content-start flex-fill nav-container">

			<a href="/" class="nav-link">
				<span class="d-none d-md-block">Documents</span>
				<i class="bi bi-folder2 nav-icon"></i>
			</a>

			<a href="/create" class="nav-link">
				<span class="d-none d-md-block">Create</span>
				<i class="bi bi-plus-circle nav-icon"></i>
			</a>

			<a href="/share" class="nav-link">
				<span class="d-none d-md-block">Share</span>
				<i class="bi bi-share nav-icon"></i>
			</a>

		</div>

		<div class="d-flex side-menu">

			<a href="/help" class="w-100 nav-link help-link justify-content-between align-items-center">
				<span class="d-none d-md-block">Need some help?</span>
				<i class="bi bi-question-circle"></i>
			</a>

		</div>

	</div>

</nav>

<style lang="scss">

  .nav-container {
    display: flex;
    justify-content: start;
	min-width: 22ch;
  }

  .nav-link {
	height: min-content;
	padding: 16px 12px;
	font-size: 20px;
	justify-content: space-between;
	i {
	  font-size: 24px;
	}
  }

	.help-link {
      height: 100%;
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
