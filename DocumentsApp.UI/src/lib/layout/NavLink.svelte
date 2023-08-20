<script lang="ts">

    export let icon: string;
    export let href: string;
    export let currentRoute: string;
    export let title = "";
	export let expand = false;
</script>

<a
	class="nav-link {expand ? 'nav-link-expand' : ''}"
	href="{href}"
	class:active={currentRoute === href}
>
	<span class="whitespace-nowrap {expand ? 'nav-link-expand' : 'hidden'}">{title}</span>
	<span class="material-icons-outlined">{icon}</span>
</a>

