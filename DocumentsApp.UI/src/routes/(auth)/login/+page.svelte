<script>
    import {LoginDto} from "$lib/models/dtos/auth/LoginDto.ts";
    import {AuthApiService} from "$lib/services/api/authApiService.ts";

    let loginDto = new LoginDto();

    async function submit() {
        await AuthApiService.login(loginDto);
	}

</script>

<div class="w-full h-full mt-[25vh]">

	<div class="flex flex-col justify-start items-center">

		<div class="rounded-lg w-min min-w-[400px] text-center bg card-shadow">

			<h1 class="text-2xl font-semibold border-b-2 border-purple-200 text-white mb-6 mx-4 py-3">Login</h1>

			<form class="flex flex-col px-4">
				<input id="username" bind:value={loginDto.email} class="form-field" type="text" placeholder="Username or email...">
				<input id="password" bind:value={loginDto.password} class="form-field" type="password" placeholder="Password...">

				<div class="mt-2 mb-4 flex justify-between items-center">
					<button type="submit" class="btn form-btn tran-75" on:click={submit}>Submit</button>
					<a href="/register" class="text-white opacity-40 hover:opacity-50 tran-75 underline">Do not have an account?</a>
				</div>

			</form>
		</div>



	</div>

</div>

<style lang="postcss">
	.bg {
		@apply bg-gradient-to-b from-prim-900 to-prim-950
		dark:bg-gradient-to-b dark:from-prim-800 dark:to-prim-900
		duration-200 ease-in-out transition-all
	}
	.form-field {
        @apply py-1 px-2 mb-4 w-full rounded border-purple-500
	}
	.form-btn {
        @apply py-1 px-6 text-white font-semibold bg-teal-500 self-center border-0
	}
</style>