import { createApp } from "vue";
import App from "./app/App.vue"; // Sử dụng App.vue làm root component
import router from "./router";
import "./style.css";

const app = createApp(App);

router.beforeEach((to, from, next) => {
  const isLoggedIn = !!localStorage.getItem("token");

  if (to.meta.requiresAuth && !isLoggedIn) {
    // Nếu route yêu cầu đăng nhập và người dùng chưa đăng nhập
    alert("You need to log in first!");
    next("/login"); // Chuyển hướng đến trang đăng nhập
  } else {
    next(); // Cho phép truy cập
  }
});

app.use(router);
app.mount("#app");
