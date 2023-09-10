<script>

    async function send() {
        await callApi(`${appSettings.apiUrl}/Auth/SignIn`, "POST", {email: "mr.grabka@gmail.com", password: "test"})
    }
</script>

Account!

<button class="btn bg-prim-200" on:click={send}></button>