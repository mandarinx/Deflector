TileMaster
By: Randy Phillips (aka Caliber Mengsk)
==================================================
	Hi, and thank you for buying my extension.
I put a lot of work into it to try and make it as
easy to use, and as stable as possible. This tool
is intended to be used to place 2d tile sets using
the built in sprite engine of unity. This allows
for using the built in batching system, as well as
any other benefits the 2d sprite engine provides.

	If this is your first time using this
extension, please watch the video tutorial here:
https://www.youtube.com/watch?v=mxy9HdNM-is

	There are a few things to point out about
this extension. This will be a small explination
of some things you need to do.


==================================================
Using the tool
==================================================
	After unpacking this tool using the unity
package, there is only really two things that you
MUST do to get your tilesets working. First, you
need to put all of the images you want to use as
tilesets into the "Resources/Tilesets" folder, and
imported as a sprite. Second thing you need to do
is to slice the tileset. Assuming it's a standards
based tileset in an exact grid, this is super easy
inside of unity. Otherwise, if they are all odd
sizes that don't fit in a grid, you can get a head
start using the automatic slicer built into unity
as well. That's it. After you've done that, it
should show up in the list of tilesets and should
be pretty easy to use past that.


==================================================
Describing parts of the tool
==================================================

Paint
--The paint tool is similar to how you would paint
a color in MS Paint or Photoshop. You select the
tile (color) and click and drag to paint it on the
currently selected layer.

Erase
--Similar to the paint tool, but removes the tiles
on the currently selected layer.

Box Paint
--If you click and drag with this selected, it will
fill the box you now see with the current tile on
the currently selected layer.

Highlight Layer
--This highlights the entire layer that's selected
with the color to the right of the check box. You
can even change the alpha level to see things behind
the current layer. Uncheck this before saving the
level or hitting play, as this could save the
hightlight directly to the layer.

Paint With Collider
--This adds a collider to every tile individually
with a custom polygon collider. This collider fixes
issues found with using the normal box collider where
players and enemies can get stuck on corners even if
they are exactly flat. In a future update, this will
be optimized to have generated colliders for the
entire layer.

Selecting multiple tiles
--You can select multiple tiles at the same time to
paint by holding the left control key and clicking.
With multiple tiles selected, both Paint and Box Paint
will randomly select a tile out of the multiple tiles
to be placed. This is great for placing things like
grass.

Layers
--The layers make painting things pretty easy. You can
toggle visibily of the entire layer, move it up and
down in the layers, rename it, and delete it. This 
will be extended upon later after other features are
added in.


==================================================
F.A.Q.
==================================================

Q: I don't see any of the images I put in as
tilesets in your extension!

A: Be certain you've imported the image as a 
sprite and that you have placed it in the 
"Resources/Tilesets" folder, and that you've got
slices inside of it.


Q: Can I paint animated objects?

A: Not at the moment. I plan to extend the tool
to allow for painting prefabs in general, so you
could even paint entire enemies with scripts, but
that's in a future update.


Q: Can I change the spacing of the tiles?

A: Not at the moment. For the time being it is a
1x1 grid inside of unity.


Q: Can I paint things that are different sizes?

A: Yes! Things like windows or trees are normally
not only a one tile sized thing. You can paint
larger objects, and it should paint it based on
the anchor point that you set when slicing the 
sprite to the right shape.


Q: When I paint things, there's sometimes a weird
line when the camera moves. What's going on?

A: This is actually a bug inside of unity due to
how sprites work. What's happening is a sprite is
actually a 2d plane in 3d space. This allows for 
both 2d and 3d objects to be in the same game easily
without having to rewrite an entire engine. The 
problem with this is that you get UV mapping glitches
due to the innaccuracy of the UV map system. I am
working on a tool that will be included in an update
that will take your slices and duplicate the outside
pixel for you and save the image out to a new image.
This fixes the problem completely, but it takes a
lot of work to do manually, and I'm having issues
with coding it. I'll make an option to overwrite the
original image and slice it in order, so you won't
even have to re-paint tiles with it. This could be
as few months down the road though. As stated, it
is part of how unity works that is causing this and
I'm working to fix it. :D Just give me a little time.


Q: Is there a way to do parallaxing?

A: Not natively, at least not yet. If you want to,
you could paint a layer to be the background (the
layer you want to parallax), and when done, just
drag the layer out of the TileMaster work parent
and use it like you would any other game object.

Q: What features do you plan to add down the line?

A: At this point I'd like to make a way to paint a
tile group (like trees) to paint all in one go
instead of one tile at a time. I'd also like to
support painting game objects with scripts attached
(like enemies, coins, death spikes, etc). This may
be a bit down the road as I'd like to get the next
feature in first. That being the image padding
function. As stated above, there is a uv bug in
unity when it comes to tiling, and the best way to
get rid of it is to pad the tile by one pixel
(duplicating the outside pixel of the tile).
Eventually, I'd also like to add layer styles like
photoshop has, but this may not be as easy of
a feature to add as I want. **NOTE** None of these
features are a promise. They are wants, and I have
no timeline for when these may be put in, if at all
but I'd like to say that I do intend to make it
even better then it already is.


==================================================
Contact
==================================================
Please send any questions or suggestions to:
Caliber_Mengsk@yahoo.com

If you need help fast, I still suggest emailing me
but you can also tweet at me on twitter:
@CaliberMengsk
https://twitter.com/CaliberMengsk

I'll reply as soon as possible.