<script>
    import {RegisterDto} from "$lib/models/dtos/auth/RegisterDto";
    import {register} from "$lib/services/api/authApiService.ts";

    let registerDto = new RegisterDto();

    async function submit() {
        await register(registerDto);
    }

</script>

<div class="w-full h-full mt-[25vh]">

	<div class="flex flex-col justify-start items-center">

		<div class="rounded-lg w-min min-w-[400px] text-center bg">

			<h1 class="text-2xl font-semibold border-b-2 border-purple-200 text-white mb-6 mx-4 py-3">Register</h1>

			<form class="flex flex-col px-4">
				<input id="username" bind:value={registerDto.userName} class="form-field" type="text" placeholder="Username...">
				<input id="email" bind:value={registerDto.email} class="form-field" type="text" placeholder="Email...">
				<input id="password" bind:value={registerDto.password} class="form-field" type="password" placeholder="Password...">
				<input id="confirmPassword" bind:value={registerDto.confirmPassword} class="form-field" type="password" placeholder="Confirm password...">

				<div class="mt-2 mb-4 flex justify-between items-center">
					<button type="submit" class="btn form-btn" on:click={submit}>Submit</button>
					<a href="/login" class="text-white opacity-40 hover:opacity-50 underline">Have an account?</a>
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