using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A simple example of an action.
/// </summary>
public class HarvestAction : AgrobotAction
{
	private AgrobotTool m_tool;

	public HarvestAction(AgrobotInteractable target, AgrobotBehaviour behaviour, AgrobotEquipment equipment) : base(target, behaviour, equipment)
	{
		m_tool = equipment.GetTool(GetFlags());
	}

	public override InteractableFlag GetFlags()
	{
		return InteractableFlag.HARVEST;
	}

	public override IEnumerator Start()
	{
		while (Vector3.Distance(m_tool.GetToolObject().transform.position, m_targetInteractable.transform.position) > 0.1f)
		{
			m_tool.GetToolObject().transform.position = Vector3.MoveTowards(
				m_tool.GetToolObject().transform.position,
				m_targetInteractable.transform.position,
				TimeChanger.DeltaTime);
			yield return null;
		}
		m_targetInteractable.OnInteract(this);
		Finish();
	}
}
