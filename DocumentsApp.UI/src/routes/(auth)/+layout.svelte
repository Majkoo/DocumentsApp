<script lang="ts">
    import "../../app.css"
    import {onMount} from "svelte";
    import {initFlowbite} from "flowbite";

    onMount(() => {
        initFlowbite();
    })
</script>

<slot/>

<style global>
    @import "../../../node_modules/material-icons";
</style>