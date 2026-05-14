# Bicep VM

## Summary

Here I fixed the Bicep error from part 4 which was finally tracked down to be an issue with circular references. It was fixed
by extracting the NSG rules from the NSG deployment so that Bicep could figure out the NSG resource id on the subnet definitions.

The problem was really that the error message was pretty misleading.

## Video

<a href="https://www.youtube.com/watch?v=999uwU0YbKY" target="_blank">
    <img src="https://img.youtube.com/vi/999uwU0YbKY/0.jpg" />
</a>
