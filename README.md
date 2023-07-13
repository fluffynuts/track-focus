track-focus
---

A small commandline app to track what app currently has focus (win32).

I needed this when something kept stealing focus every 10-20s and I 
couldn't figure out what it was - turns out it was a subprocess from 
Docker Desktop (mstsc) and restarting Docker Desktop fixes it, but I 
was getting this frequently enough that was getting really annoying.
