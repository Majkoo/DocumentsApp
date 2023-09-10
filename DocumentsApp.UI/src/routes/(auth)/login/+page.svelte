<script>
    import {LoginDto} from "$lib/models/dtos/auth/LoginDto";
    import {login} from "$lib/services/api/authApiService.ts";

    let loginDto = new LoginDto();

    async function submit() {
        await login(loginDto);
	}

</script>

<div class="w-full h-full mt-[25vh]">

	<div class="flex flex-col justify-start items-center">

		<div class="rounded-lg w-min min-w-[400px] text-center bg">

			<h1 class="text-2xl font-semibold border-b-2 border-purple-200 text-white mb-6 mx-4 py-3">Login</h1>

			<form class="flex flex-col px-4">
				<input id="username" bind:value={loginDto.email} class="form-field" type="text" placeholder="Username or email...">
				<input id="password" bind:value={loginDto.password} class="form-field" type="password" placeholder="Password...">

				<div class="mt-2 mb-4 flex justify-between items-center">
					<button type="submit" class="btn form-btn" on:click={submit}>Submit</button>
					<a href="/register" class="text-white opacity-40 hover:opacity-50 underline">Do not have an account?</a>
				</div>

			</form>
		</div>



	</div>

</div>

<style lang="postcss">
	.bg {
		@apply bg-gradient-to-b from-surface-800 to-surface-900
		dark:bg-gradient-to-b dark:from-surface-800 dark:to-surface-900
		duration-200 ease-in-out transition-all
	}
	.form-field {
        @apply py-1 px-2 mb-4 w-full rounded border-primary-500
	}
	.form-btn {
        @apply py-1 px-6 text-white font-semibold bg-primary-500 self-center border-0
	}
</style>