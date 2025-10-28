# Developer Guide

1. Platform: Build target x64.
2. References: Add TaleWorlds.* from local Bannerlord installation. Set Copy Local = False.
3. Build: Rebuild Solution (x64).
4. Deployment: Run tools/PostBuildCopy.ps1 or copy DLL to Documents/.../Modules/WarCouncilModern/bin/x64/.
5. Testing: Follow TEST_PLAN.md.