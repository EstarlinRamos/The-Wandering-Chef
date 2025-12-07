VAR pilot_name = "Reyes"
VAR synchrony_score = 0
VAR concord_reputation = 0
VAR asked_jurrac = false

-> start

=== start ===
You stand in Observation Deck Twelve of the Concord Citadel, a moon sized station that hangs above the Triarch Nebula. 
Human officers in crisp USF uniforms move beside robed Serf envoys and a low murmur of many languages vibrates in the air.

Today is the day of your Synchrony Protocol evaluation.
If your neural pattern resonates with a Genesis Frame you will join the new generation of pilots.
If it fails you go back to support duty.

Your name on the roster reads {pilot_name}. The display waits while you steady your hands.

You can stall for a moment or step through the doors into the evaluation chamber.

* Step through the doors without looking back.
    -> briefing
* Pause and study the people around you.
    -> meet_observers
* Ask one of the officers what really happened in the Jurrac War.
    -> ask_jurrac_from_start

=== ask_jurrac_from_start ===
You catch a passing lieutenant who wears old campaign bars that glow a faint orange.

"Sir, you fought in the Jurrac War, right"
"What was it actually like"

He studies you for a long second, then leans closer.

"It was not like the clean holoreels. The sky burned for months.
Cities turned into glass. We learned to build mecha because the old ways of fighting stopped working.
Remember that when they slide you into that pod, cadet.
Those machines exist because we had no other choice."

The weight of his words settles on your shoulders as the station lights dim for cycle change.

~ asked_jurrac = true
-> start_after_jurrac

=== start_after_jurrac ===
You return your focus to the glowing doors of the evaluation wing.
The Synchrony Protocol will begin as soon as you step inside.

* Step through the doors without looking back.
    -> briefing
* Take one last look at the nebula through the observation glass.
    -> reflection

=== meet_observers ===
You slow down and let the flow of traffic move around you.

At the center of the deck the USF Chief Ambassador Veronica Rivera speaks with a tall Serf woman whose skin carries a faint shimmer.
You recognize her from every news feed: Princess Eryndra Valesar of Gaia.

You edge a little closer and catch the end of their conversation.

Veronica says, "If Synchrony spreads beyond human pilots, the Concord will need a new charter."
Eryndra answers in a cool musical tone. "Or perhaps new guardians."

Veronica notices you first.

"Cadet, you are on the roster for the Genesis trials, correct"
"You have time for a question or two."

* Answer Veronica with strict formality.
    -> talk_veronica_formal
* Answer with easy confidence and a grin.
    -> talk_veronica_casual
* Ignore the invitation and hurry into the evaluation wing.
    -> briefing

=== talk_veronica_formal ===
You snap to attention.

"Yes maam. Cadet {pilot_name}. Synchrony candidate."

Veronica smiles, amused but approving.

"Protocol is charming in small doses.
Tell me this, cadet. Are you volunteering because you want power or because someone has to stand in front when the next war comes."

* "Someone has to stand in front."
    ~ synchrony_score += 1
    ~ concord_reputation += 1
    -> after_ambassadors
* "Power. I want to see what a Genesis Frame can really do."
    ~ synchrony_score += 1
    -> after_ambassadors

=== talk_veronica_casual ===
You offer a quick half salute.

"That is me. Hoping the machine likes my brain."

Veronica snorts softly and Eryndra tilts her head with curious interest.

Eryndra asks, "And what do you hope to do with that bond if it forms, little star"

* "Keep Earth safe. That is the whole point."
    ~ synchrony_score += 1
    ~ concord_reputation += 1
    -> after_ambassadors
* "See the galaxy and win every simulator tournament on the station."
    ~ synchrony_score += 1
    -> after_ambassadors

=== after_ambassadors ===
Veronica waves you away with a quick gesture.

"Go. If Synchrony accepts you, we will talk again under different lights."

The Princess studies you as you leave.
You feel a faint pressure at the back of your thoughts, as if a distant song just noticed your presence.

-> briefing

=== reflection ===
You stand before the vast observation window.
The Triarch Nebula blooms in colors your eyes can barely parse, stitched with silent lightning.

For a moment you imagine the first Jurrac ships sliding out of that glow, and the first human mecha rising to meet them.

Whatever happens in the chamber, this is the sky you are choosing to defend.

-> briefing

=== briefing ===
The doors hiss apart and you enter a chamber lit in cold blue.

A technician in a USF jumpsuit checks a tablet, then looks up.

"Cadet {pilot_name}. Good. You are on time.
This will be simple. You will lie in the cradle.
We engage the Synchrony lattice.
You keep your mind steady.
You will feel the Genesis core reach back."

Behind the glass wall you see the frame itself.
Humanoid, smooth armored plates, spine socket bristling with interfaces.
The eyes are dark, waiting.

* Focus on your breathing and clear your mind.
    -> sync_calm
* Let your excitement surge. You have waited your whole life for this.
    -> sync_hype
* Reach out with your thoughts, trying to feel the machine first.
    -> sync_reach

=== sync_calm ===
You count each breath as you settle into the cradle.
Sensors cling to your skin.
The pod slides shut, cutting off the outside noise.

Inside the black you find a point of stillness and hold it.

The moment the Synchrony field engages, something brushes the edge of your thoughts and follows that stillness inward.

~ synchrony_score += 2
-> sync_link

=== sync_hype ===
Your heart slams with adrenal fire as the cradle closes.
This is it.
Every simulation, every briefing, every café argument about frame loadouts led to this single plunge.

The Synchrony field snaps on like a storm.
Your racing thoughts crash into it and scatter.

~ synchrony_score -= 1
-> sync_link

=== sync_reach ===
You do not wait for the field to wash over you.
Instead you reach forward, imagining your awareness spilling down the cables into the waiting frame.

For a second nothing happens.

Then something old and brilliant and curious turns its attention toward you.

~ synchrony_score += 1
-> sync_link

=== sync_link ===
For a few timeless heartbeats there is no station, no cradle, no body.

There is only you and the presence on the other side of the lattice.
It feels vast but focused, like a storm forced into the shape of a blade.

You sense questions without words.
You answer with impressions: duty, fear, hunger for the stars, whatever rises first in your chest.

The field collapses in a flash of white.

You gasp as the cradle opens and the technicians rush in.

{ synchrony_score >= 2:
    -> result_high
}
{ synchrony_score <= 0:
    -> result_low
}
-> result_mid

=== result_high ===
The technician stares at the readings with wide eyes.

"Full resonance on a first attempt.
Admiral Knight will want this report in person."

Veronica appears at the observation glass with a small satisfied smile.
Behind her, Princess Eryndra watches you like a scientist who has just discovered a dangerous new element.

The Genesis Frame on the other side of the chamber opens its eyes.
You feel that unseen presence settle around your thoughts like a mantle.

"You belong to the front line now, cadet," the technician says quietly.

To be continued in your next scene, where you and your frame take your first real mission.

-> END

=== result_mid ===
The readings flicker, then stabilize.

"Partial resonance," the technician mutters.
"Stable enough to train, but the lattice hesitated."

The chamber crew look at you with a mix of relief and curiosity.

From the observation glass Veronica gives you a short nod.
Not disappointment, not celebration, just a promise that your story is not decided yet.

"You will start with simulator duty and controlled sorties," the technician says.
"Do not waste this chance.
Very few people touch Synchrony at all."

You leave the chamber with your nerves buzzing and the faint memory of another mind brushing yours.

-> END

=== result_low ===
The Synchrony field snaps off almost as soon as it begins.

Pain lances through your skull and you struggle to sit up as alarms blink red across the chamber.

The technician curses softly.

"Rejection. Hard rejection.
The core pushed back.
We will need to recalibrate the lattice before the next candidate."

From behind the glass Veronica speaks into the intercom.

"Cadet {pilot_name}, this result does not end your service to the Concord.
It only redirects it."

Princess Eryndra narrows her eyes, as if she felt something in the failure that no one else did.

As medics guide you from the cradle you feel a flicker of something stubborn inside you.
The core rejected you today, but that does not mean the story is over.

You step out of the chamber with your future blurred and new questions forming.

-> END
