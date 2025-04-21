<template>
  <footer class="bg-slate-800 text-white p-4 w-full">
    <div class="flex flex-col items-center justify-center">
      <div
        class="container mx-auto flex justify-center space-x-15 items-center mt-5"
      >
        <router-link to="/">
          <img
            src="../assets/Logo.png"
            alt="Logo"
            class="h-20 border-r border-gray-400 pr-14"
          />
        </router-link>
        <p class="font-semibold w-25">Virtual Class for Zoom</p>
      </div>
      <div class="mt-10 w-[500px] flex flex-col justify-center items-center">
        <p class="p-3 text-center text-slate-400">
          Subscribe to our Newsletter
        </p>
        <div>
          <input
            type="email"
            placeholder="Your email"
            class="p-2 border border-slate-400 rounded-[33px] pl-4 mr-4 w-[300px]"
          />
          <button
            class="p-2 bg-teal-400 rounded-[33px] text-white hover:bg-teal-500 cursor-pointer pl-4 pr-4"
          >
            Subscribe
          </button>
        </div>
      </div>
      <div class="mt-10">
        <a
          class="border-r border-gray-400 pl-2 pr-2 hover:text-teal-400 hover:underline"
          href="#"
          >Careers</a
        >
        <a
          class="border-r border-gray-400 pl-2 pr-2 hover:text-teal-400 hover:underline"
          href="#"
          >Privacy Policy</a
        >
        <a class="pl-2 pr-2 hover:text-teal-400 hover:underline" href="#"
          >Terms and Conditions</a
        >
        <p class="text-center mt-4">Copyright &copy; 2023 IELTS Talent</p>
      </div>
    </div>
  </footer>
</template>
