<script lang="ts">
	import ThemeSwitch from "$lib/components/layout/ThemeSwitch.svelte";
	import NavLink from "$lib/components/layout/NavLink.svelte";
	import {appSettings} from "$lib/configs/appSettings";

    export let currentRoute: string;
    let expand = false;
</script>

<aside class="nav {expand ? 'nav-expand' : 'nav-collapse'}">

	<div class="nav-title-section {expand ? 'flex-row justify-between' : 'flex-col items-center'}">

		<a class="flex justify-between items-center w-min self-center p-2.5 font-bold select-none rounded-lg cursor-pointer gap-2 flex-nowrap" href="/static">
			<span class="material-icons-outlined">
			description
			</span>
			{#if expand} <h1 class="font-bold text-lg">{appSettings.appTitle}</h1> {/if}
		</a>

		<button type="button" class="nav-link" on:click={() => {expand = !expand}}>
			<span class="material-icons-outlined">{expand ? 'chevron_left' : 'chevron_right'}</span>
		</button>

	</div>

	<div class="nav-link-section flex-grow">
		<NavLink icon="folder" currentRoute="{currentRoute}" href="/my-items" title="My items" expand="{expand}"/>
		<NavLink icon="add" currentRoute="{currentRoute}" href="/create" title="Create" expand="{expand}"/>
		<NavLink icon="share" currentRoute="{currentRoute}" href="/share" title="Share" expand="{expand}"/>
	</div>

	<div class="nav-link-section">
		<NavLink icon="question_mark" currentRoute="{currentRoute}" href="/help" title="Help" expand="{expand}"/>
		<ThemeSwitch expand="{expand}" />
	</div>

	<div class="nav-account-section pt-2 ">
		<NavLink icon="person" currentRoute="{currentRoute}" href="/account" title="Account" expand="{expand}"/>
	</div>
</aside>