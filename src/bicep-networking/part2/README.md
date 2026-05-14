# Bicep Networking Part 2: "Virtual Machines the right way"

## Videos

<a href="https://www.youtube.com/watch?v=9rQH20bLj78" target="_blank">
    <img src="https://img.youtube.com/vi/9rQH20bLj78/0.jpg" />
</a>

<a href="https://www.youtube.com/watch?v=_ihUpx6n-vc" target="_blank">
    <img src="https://img.youtube.com/vi/_ihUpx6n-vc/0.jpg" />
</a>

## Summary

After the first part I now deploy the first VM. This deploys:

- Network Security Group
- Network Interface Card
- Managed Disc
- Virtual Machine (Windows 11 from Marketplace)
- Public IP
- Auto-Shutdown

Because Bicep can get a little tricky on all the options I also created a simple container-tool [AzureVmOfferHelper](https://youtu.be/_ihUpx6n-vc) which allows you to setup the image definition pretty easily.

Also I will complain a lot about MS module quality (I think rightfully so). I think this
is actually good for you as a watcher so that you can see where the pitfalls are.

In the end we will have a Windows 11 VM with niceley named NSG, NIC, Disc and an auto-shutdown
set up all defined in Bicep.

In this video I'm not going to connect to the machine. This and other stuff is coming in the
next video.
