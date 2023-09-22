import { ps,  task, parseAndRun, env, path } from "https://deno.land/x/qtr@0.0.5/mod.ts";

const cwd = path.dirname(path.fromFileUrl(import.meta.url));
const rootDir = path.resolve(path.join(cwd, ".."));
const sln = path.join(rootDir, "dfx.sln");
const tplDir = path.join(rootDir, "tpl");
const tf = env.getOrDefault("TF_BUILD", "false") === "true";
const ci = env.getOrDefault("CI", "false") === "true" || tf;
const dconfig = env.getOrDefault("DOTNET_BUILD_CONFIG", ci ? "Release" : "Debug");
const tpls = ["Bearz.Library", "Bearz.Console", "Bearz.Xunit.Library"];

task("clean", () => {
    return ps.exec("dotnet", ["clean", sln, "-c", dconfig]);
});

task("restore", () => {
    return ps.exec("dotnet", ["restore", sln, "-c", dconfig]);
});

task("build", () => {
    const args = ["build", sln, "-c", dconfig];
    if (ci)
        args.push("--no-restore");

    return ps.exec("dotnet", args);
});

task("test", () => {
    const args = ["test", sln, "-c", dconfig];
    if (ci) {
        args.push("--no-restore");
        args.push("--no-build");
    }

    return ps.exec("dotnet", args);
});

task("lib:add", () => {
    const lib = env.getRequired("LIB");
    const dest = `./src/${lib}`
    ps.execSync("dotnet", ["new", "bearz-lib", "-n", `Bearz.${lib}`, "-o", dest], { cwd: rootDir });
    ps.execSync("dotnet", ["sln", ".", "add", dest], { cwd: rootDir })
})

task("xunit:add", () => {
    const lib = env.getRequired("LIB");
    const dest = `./test/${lib}.Tests`
    ps.execSync("dotnet", ["new", "bearz-xunit", "-n", `Bearz.${lib}.Tests`, "-o", dest], { cwd: rootDir });
    ps.execSync("dotnet", ["sln", ".", "add", dest], { cwd: rootDir })
})

task("tpl:add", () => {
  
    for (const tpl of tpls) {
        const r = ps.execSync("dotnet", ["new", "install", "--force", path.join(tplDir, tpl)]);
        r.throwOrContinue();
    }
});

task("tpl:rm", () => {
    for (const tpl of tpls) {
        const r = ps.execSync("dotnet", ["new", "uninstall", path.join(tplDir, tpl)]);
        r.throwOrContinue();
    }
});

if (import.meta.main) {
    const exitCode = await parseAndRun(Deno.args);
    Deno.exit(exitCode);
}