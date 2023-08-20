<script lang="ts">
	import ThemeSwitch from "$lib/layout/ThemeSwitch.svelte";
	import NavLink from "$lib/layout/NavLink.svelte";

    export let currentRoute: string;
    let expand = false;
</script>

<nav class="nav {expand ? 'nav-expand' : 'nav-collapse'}">

	<button class="btn text-white" on:click={() => {expand = !expand}}>exp</button>

	<div class="nav-mandatory-links">
		<NavLink icon="folder" currentRoute="{currentRoute}" href="/my-items" title="My items" expand="{expand}"/>
		<NavLink icon="add" currentRoute="{currentRoute}" href="/create" title="Create" expand="{expand}"/>
		<NavLink icon="share" currentRoute="{currentRoute}" href="/share" title="Share" expand="{expand}"/>
	</div>

	<NavLink icon="question_mark" currentRoute="{currentRoute}" href="/help" title="Help" expand="{expand}"/>
	<ThemeSwitch />
	<NavLink icon="person" currentRoute="{currentRoute}" href="/account" title="Account" expand="{expand}"/>
</nav>