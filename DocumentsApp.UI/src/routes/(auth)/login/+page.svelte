<script>
	import {appSettings} from "$lib/configs/appSettings.ts";
</script>

<div class="w-full h-screen mt-[25vh]">

	<div class="flex flex-col justify-start items-center">

		<a class="flex justify-between items-center w-min self-center mb-4 select-none rounded-lg cursor-pointer gap-2 flex-nowrap text-4xl" href="/">
			<span class="material-icons-outlined icon-lg">description</span>
			<div class="font-semibold">{appSettings.appTitle}</div>
		</a>

		<div class="rounded-lg w-min mb-20 min-w-[400px] text-center bg">

			<h1 class="text-xl border-b-2 border-purple-200 text-white py-2 mb-4">Log in to your account</h1>

			<div class="flex flex-col px-4 py-2">
				<input id="username" class="py-1 px-2 mb-4 w-full rounded border-purple-500" type="text" placeholder="Username or email...">
				<input id="password" class="py-1 px-2 mb-4 w-full rounded border-purple-500" type="password" placeholder="Password...">
				<button type="submit" class="py-1 px-6 mb-4 text-white font-semibold btn bg-teal-500 self-center">Submit</button>
			</div>
		</div>

	</div>

</div>

<style lang="postcss">
	.bg {
		@apply bg-gradient-to-b from-prim-900 to-prim-950
		dark:bg-gradient-to-b dark:from-prim-800 dark:to-prim-900
		duration-200 ease-in-out transition-all
	}
	.icon-lg {
		@apply text-5xl !important
	}
</style>